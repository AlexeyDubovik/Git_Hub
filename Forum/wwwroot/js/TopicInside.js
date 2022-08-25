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
    buttonPublish.onclick = buttonPublishClick;
    getAll(Data.TopicId, '/templates/Article.html', '/templates/Quote.html').then(([Articles, templatePost, templateQuote]) => {
        if (Articles instanceof Array) {
            Data.templatePost = templatePost;
            Data.templateQuote = templateQuote;
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
function getAll(TopicID, Path_A, Path_Q) {
    return Promise.all([GetArticles(TopicID), GetHTML(Path_A), GetHTML(Path_Q)]);
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
        let text = "";
        if (Article.reply !== null) {
            const reg = new RegExp("{{QuotedMessage}}");
            Data.Quote = QuoteShaper(Data, Article.reply);
            if (reg.test(Article.text)) {
                console.log(Article);
                text = Article.text.replaceAll("{{QuotedMessage}}", Data.Quote);
            }
        }
        else {
            text = Article.text;
        }
        let HTML = Data.templatePost
            .replaceAll("{{ArticleID}}", Article.id)
            .replaceAll("{{RealName}}", Article.author.realName)
            .replaceAll("{{Atribute}}", Article.author.id != Article.topic.author.id ? "" : `<i style="color:yellow;">&#9733</i>`)
            .replaceAll("{{Avatar}}", img.outerHTML)
            .replaceAll("{{dateHref}}", '#')
            .replaceAll("{{dateValue}}", new Date(Article.createdDate).toLocaleString(Data.Local))
            .replaceAll("{{ReplyHref}}", '#Article_Textarea')
            .replaceAll("{{postText}}", text)
            .replaceAll("{{ProfileHref}}", '#')
            .replaceAll("{{PMHref}}", '#');
        Data.Table.innerHTML += HTML;
        if (Article.reply !== null) {
            const Quote = document.getElementById(`${Article.id}`).querySelector(".Quote");
            modalQuote(Quote);
        }
    }
    const txtb = document.querySelectorAll("ul li .txtb");
    for (let href of txtb)
        href.onclick = ReplyClick(Data);
}
function modalQuote(Reply) {
    let txt = Reply.querySelector(".hiddenMessage").innerText;
    txt = txt.trimStart();
    let length = txt.length;
    if (length < 15) {
        console.log(1);
        return;
    }
    Reply.addEventListener("mouseover", (e) => {
        const QuoteM = e.target.closest('.quoteMessage');
        let win = document.querySelector('#modal');
        if (win !== null) {
            win.innerHTML = QuoteM.querySelector(".hiddenMessage").innerText;
            let x = ev.clientX + pageXOffset;
            let y = ev.clientY + pageYOffset;
            win.style.left = x + 20 + 'px';
            win.style.top = y + 'px';
            return;
        }
        try {
            win = document.createElement('div');
            win.innerHTML = QuoteM.querySelector(".hiddenMessage").innerText;
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
    Reply.addEventListener("mouseout", () => {
        try {
            document.querySelector('#modal').remove();
        }
        catch {

        }
    });
}
function ReplyClick(Data) {
    return (e) => {
        try {
            Data.PreView.querySelector(".Quote").remove();
        }
        finally {

            const tbody = e.target.closest('tbody');
            const ReplyID = `${tbody.getAttribute("id")}`;
            const Article = Data.Articles.filter((element) => {
                return element.id.indexOf(ReplyID) > -1;
            });
            Data.Textarea.value = "{{QuotedMessage}}";
            const reg = new RegExp("{{QuotedMessage}}");
            if (reg.test(Article[0].text)) {
                Article[0].text = Article[0].text.replaceAll("{{QuotedMessage}}", "");
            }
            Data.Quote = QuoteShaper(Data, Article[0]);
            Data.PreView.innerHTML += Data.Quote;
            const Quote = Data.PreView.querySelector(".Quote");
            modalQuote(Quote);
            Data.PreViewHidde();
            Data.Textarea.setAttribute("data-reply-id", ReplyID);
        }
    }
}
function QuoteShaper(Data, Article) {
    let str = "";
    if (Article.text.length > 15) {
        str = Article.text.substr(0, 15);
        str += "...";
    }
    else {
        str = Article.text;
    }
    let HTML = Data.templateQuote
        .replaceAll("{{ReplyID}}", Article.id)
        .replaceAll("{{QuotedFrom}}", Article.author.realName)
        .replaceAll("{{Q_LocaleText}}", 'quoted:')
        .replaceAll("{{QuoteMessage}}", Article.text.trimStart())
        .replaceAll("{{showMessage}}", str);
    return HTML;
}
function TextAreaKeyUp(Data) {
    return (e) => {
        Data.PreViewHidde();
        Data.PreView.innerText = e.target.value;
        const reg = new RegExp("{{QuotedMessage}}");
        if (Data.Quote !== "" && reg.test(Data.Textarea.value)) {
            let html = Data.PreView.innerText
                .replaceAll("{{QuotedMessage}}", Data.Quote);
            Data.PreView.innerHTML = html;
        }
        else
            try {
                Data.textarea.removeAttribute("data-reply-id");
            }
            catch {
            }
    }
}
function TextAreaChange(Data) {
    return (e) => {
        Data.PreViewHidde();
        Data.PreView.innerText = e.target.value;
        const reg = new RegExp("{{QuotedMessage}}");
        if (Data.Quote !== "" && reg.test(Data.Textarea.value)) {
            let html = Data.PreView.innerText
                .replaceAll("{{QuotedMessage}}", Data.Quote);
            console.log(Data.Quote);
            Data.PreView.innerHTML = html;
        }
        else
            Data.textarea.removeAttribute("data-reply-id");
    }
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
async function postArticle(textarea) {
    const formData = new FormData();
    let ReplyId;
    try {
        ReplyId = textarea.getAttribute("data-reply-id");
    }
    catch {
        ReplyId = null;
    }
    formData.append('TopicId', `${textarea.getAttribute("data-topic-id")}`);
    formData.append('ReplyId', `${ReplyId}`);
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
