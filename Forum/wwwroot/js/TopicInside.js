import { GetHTML, GetArticles } from '/js/FetchRequest.js';
document.addEventListener("DOMContentLoaded", () => {
    const [html] = document.getElementsByTagName("html")
    const Data = {
        Table: document.getElementById("TopicArticles"),
        Locale: html.getAttribute("lang"),
        TopicId: document.getElementById("TopicArticles").getAttribute("data-topic-id"),
        Count:0
    };
    const buttonPublish = document.getElementById("button-publish");
    if (!buttonPublish) throw "button-publish element not found";
    buttonPublish.onclick = buttonPublishClick;
    getAll(Data.TopicId, '/templates/Article.html').then(([Articles, templatePost]) => {
        if (Articles instanceof Array) {
            Data.templatePost = templatePost;
            Data.Articles = Articles;
            Data.Count = Object.keys(Articles).length;
            FilterArticles(Data);
        }
        else {
            throw "ShowPost: data Invalid";
        }

    });
});
function getAll(TopicID, Path) {
    return Promise.all([GetArticles(TopicID), GetHTML(Path)])
}
function FilterArticles(Data) {
    injectArticles(Data);
    setInterval(() => GetArticles(Data.TopicId).then((Articles) => {
        if (Data.Count < Object.keys(Articles).length) {
            let ArticlesTMP = [];
            for (let i = Data.Count; i < Object.keys(Articles).length; i++) {
                ArticlesTMP.push(Articles[i]);
            }
            Data.Count = Object.keys(Articles).length;
            Data.Articles = ArticlesTMP;
            injectArticles(Data);
        }
    }), 1000);
}
function injectArticles(Data) {
    for (let Article of Data.Articles) {
        const img = new Image(100, 100);
        img.src = `/img/${Article.author.avatar}`;
        let HTML = Data.templatePost
            .replaceAll("{{post}}", Article.id)
            .replaceAll("{{RealName}}", Article.author.realName)
            .replaceAll("{{Atribute}}", Article.author.id != Article.topic.author.id ? "" : `<i style="color:yellow;">&#9733</i>`)
            .replaceAll("{{Avatar}}", img.outerHTML)
            .replaceAll("{{dateHref}}", 'none')
            .replaceAll("{{dateValue}}", new Date(Article.createdDate).toLocaleString(Data.Local))
            .replaceAll("{{ReplyHref}}", '#Article_Textarea')
            .replaceAll("{{postText}}", Article.text)
            .replaceAll("{{Profile}}", 'none')
            .replaceAll("{{PM}}", 'none')
        Data.Table.innerHTML += HTML;
    }
    const txtb = document.querySelectorAll("ul li .txtb");
    for (let href of txtb)
        href.onclick = ReplyClick;
}
function ReplyClick(e) {
    const tbody = e.target.closest('tbody');
    const post = tbody.querySelector('.post_body');
    console.log(tbody.getAttribute("id"));
    console.log(post.innerText);
}
function buttonPublishClick(e) {
    const articalText = document.querySelector("textarea");
    if (!articalText) throw "Article Textarea element not found";
    postArticle(articalText)
        .then((res) => {
            articalText.value = "";
            console.log(res);
        });
}
async function postArticle(articalText) {
    let formData = new FormData();
    formData.append('TopicId', `${articalText.getAttribute("data-topic-id")}`);
    formData.append('Text', `${articalText.value}`);
    const response = await fetch('/api/Article', {
        method: 'POST',
        headers: {
            'AuthorId': `${articalText.getAttribute("data-author-id")}`
        },
        body: formData
    });
    return await response.json();
}
