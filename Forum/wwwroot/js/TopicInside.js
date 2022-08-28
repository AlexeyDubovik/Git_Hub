import { GetHTML, GetArticles } from '/js/FetchRequest.js';
document.addEventListener("DOMContentLoaded", () => {
    const [html] = document.getElementsByTagName("html")
    const Data = {
        PreView: document.querySelector(".PreView"),
        Textarea: document.querySelector("textarea"),
        Table: document.getElementById("TopicArticles"),
        Locale: html.getAttribute("lang"),
        TopicId: document.getElementById("TopicArticles").getAttribute("data-topic-id"),
        Quote: "",
        Count: 0,
        PreViewHidde() {
            if (this.PreView.innerHTML.length === 0) {
                this.Quote = "";
                this.PreView.style = "display:none";
            }
            else
                this.PreView.style = "display:block";
        }
    };
    const buttonPublish = document.getElementById("button-publish");
    if (!buttonPublish) throw "button-publish element not found";
    buttonPublish.onclick = buttonPublishClick(Data);
    getAll(Data.TopicId,
        '/templates/Article.html',
        '/templates/Quote.html',
        '/templates/ArticleEditButton.html',
        '/templates/ArticleDeleteButton.html')
        .then(([
            Articles,
            templatePost,
            templateQuote,
            templateAEB,
            templateADB]) => {
            if (Articles instanceof Array) {
                console.log(Articles);
                Data.templatePost = templatePost;
                Data.templateQuote = templateQuote;
                Data.templateADB = templateADB;
                Data.templateAEB = templateAEB;
                Data.Articles = Articles;
                Data.Count = Object.keys(Articles).length;
                Data.Textarea.onkeyup = TextAreaKeyUp(Data);
                Data.Textarea.onchange = TextAreaChange(Data);
                FilterArticles(Data);
            }
            else {
                throw "ShowPost: data Invalid";
            }
        });
});

function FilterArticles(Data) {
    injectArticles(Data);
    setInterval(() => GetArticles(Data.TopicId).then((Articles) => {
        if (Data.Count != Object.keys(Articles).length) {
            //let ArticlesTMP = [];
            //for (let i = Data.Count; i < Object.keys(Articles).length; i++) {
            //    ArticlesTMP.push(Articles[i]);
            //}
            //Data.Articles = ArticlesTMP;
            Data.Count = Object.keys(Articles).length;
            Data.Articles = Articles;
            Data.Table.innerHTML = "";
            injectArticles(Data);
        }
    }), 1000);
}
function injectArticles(Data) {
    for (let Article of Data.Articles) {
        const img = new Image(100, 100);
        img.src = `/img/${Article.author.avatar}`;
        let text = `<p>${Article.text.replaceAll("\n", "<br>")}</p>`;
        let ButtonDelete = Data.templateADB.replaceAll("{{DeleteHref}}", "#");
        let ButtonEdit = Data.templateAEB.replaceAll("{{EditHref}}", "#");
        let HTML = Data.templatePost
            .replaceAll("{{ArticleID}}", Article.id)
            .replaceAll("{{RealName}}", Article.author.realName)
            .replaceAll("{{Atribute}}", Article.author.id != Article.topic.author.id ? "" : `<i style="color:yellow;">&#9733</i>`)
            .replaceAll("{{Avatar}}", img.outerHTML)
            .replaceAll("{{dateHref}}", '#')
            .replaceAll("{{dateValue}}", new Date(Article.createdDate).toLocaleString(Data.Local))
            .replaceAll("{{ReplyHref}}", '#Article_Textarea')
            .replaceAll("{{ArticleButtons}}", Article.replys.length === 0 ? ButtonEdit + ButtonDelete : "")
            .replaceAll("{{postText}}", text)
            .replaceAll("{{ProfileHref}}", '#')
            .replaceAll("{{PMHref}}", '#');
        Data.Table.innerHTML += HTML;
    }
    const DeletesButtons = document.querySelectorAll(".delete");
    const ReplysButtons = document.querySelectorAll(".reply");
    for (let a of DeletesButtons)
        a.onclick = DeleteClick();
    for (let a of ReplysButtons)
        a.onclick = ReplyClick(Data);
}

//
//Event
//

function ReplyClick(Data) {
    return (e) => {
        const tbody = e.target.closest('tbody');
        const ReplyID = `${tbody.getAttribute("id")}`;
        const Article = Data.Articles.filter((element) => {
            return element.id.indexOf(ReplyID) > -1;
        });
        Data.Textarea.value += Data.Textarea.value.length === 0 ?
            "{{QuotedMessage}}" : "\n" + "{{QuotedMessage}}";
        const reg = new RegExp("{{QuotedMessage}}");
        if (reg.test(Article[0].text)) {
            Article[0].text = Article[0].text.replaceAll("{{QuotedMessage}}", "");
        }
        Data.Textarea.dispatchEvent(new Event('change'));
        Data.Textarea.setAttribute("data-reply-id", ReplyID);
    }
}

function DeleteClick() {
    return (e) => {
        const tbody = e.target.closest('tbody');
        const ReplyID = `${tbody.getAttribute("id")}`;
        console.log(ReplyID);
    }
}

function TextAreaKeyUp(Data) {
    return (e) => {
        e.target.dispatchEvent(new Event('change'));
    }
}

function TextAreaChange(Data) {
    return (e) => {
        Data.PreView.innerHTML = e.target.value.length === 0 ? "" :
            `<p>${e.target.value
                .replaceAll("\n", "<br>")}</p>`;
        Data.PreViewHidde();
    }
}

function buttonPublishClick(Data) {
    return (e) => {
        if (!Data.Textarea) throw "Article Textarea element not found";
        const reg = new RegExp("{{QuotedMessage}}");
        if (!reg.test(Data.Textarea.value)) {
            Data.Textarea.removeAttribute("data-reply-id");
        }
        postArticle(Data.Textarea)
            .then((res) => {
                Data.Textarea.value = "";
                Data.PreView.innerText = "";
                Data.PreViewHidde();
                console.log(res);
            });

    }
}

//
//Request
//

async function postArticle(textarea) {
    const formData = new FormData();
    try {
        const replyId = textarea.getAttribute("data-reply-id");
        if (replyId)
            formData.append('ReplyId', `${replyId}`);
    }
    finally {
        formData.append('TopicId', `${textarea.getAttribute("data-topic-id")}`);
        formData.append('Text', `${textarea.value}`);
        const response = await fetch('/api/Article', {
            method: 'POST',
            headers: {
                'AuthorId': `${textarea.getAttribute("data-author-id")}`
            },
            body: formData
        });
        return await response.json();
    }
}

function getAll(TopicID, Path_A, Path_Q, Path_AEB, Path_ADB) {
    return Promise.all([
        GetArticles(TopicID),
        GetHTML(Path_A),
        GetHTML(Path_Q),
        GetHTML(Path_AEB),
        GetHTML(Path_ADB)
    ]);
}

//
//Qupte "Цитаты"
//

function modalQuote(Quote) {
    let txt = Quote.querySelector(".hiddenMessage").innerText;
    txt = txt.trimStart();
    let length = txt.length;
    if (length < 15) {
        return;
    }
    Quote.addEventListener("mouseover", (e) => {
        const QM = e.target.closest('.quoteMessage');
        try {
            let win = document.createElement('div');
            win.innerHTML = QM.querySelector(".hiddenMessage").innerText;
            win.id = 'modal';
            document.body.append(win);
            let ev = window.event || e;
            let x = ev.clientX + pageXOffset;
            let y = ev.clientY + pageYOffset;
            win.style.left = x + 20 + 'px';
            win.style.top = y + 'px';
            ev.preventDefault();
        } catch {

        }
    });
    Quote.addEventListener("mouseout", () => {
        try {
            document.querySelector('#modal').remove();
        }
        catch {

        }
    });
}

function QuoteShaper(tempHTML, Article) {
    let str = "";
    if (Article.text.length > 15) {
        str = Article.text.substr(0, 15);
        str += "...";
    }
    else {
        str = Article.text;
    }
    let HTML = tempHTML
        .replaceAll("{{ReplyID}}", Article.id)
        .replaceAll("{{QuotedFrom}}", Article.author.realName)
        .replaceAll("{{Q_LocaleText}}", 'quoted:')
        .replaceAll("{{QuoteMessage}}", Article.text.trimStart())
        .replaceAll("{{showMessage}}", str);
    return HTML;
}