import { GetHTML, GetTopics } from '/js/FetchRequest.js';
document.addEventListener("DOMContentLoaded", () => {
    const [html] = document.getElementsByTagName("html")
    const Data = {
        tbody: document.getElementById("Forum").getElementsByTagName("tbody")[0],
        Locale: html.getAttribute("lang"),
        date: "00.00.0000, 00:00"
    }
    getAll('/templates/Topic.html').then(([Topics, templateTopic]) => {
        if (Topics instanceof Array) {
            Data.Topics = Topics;
            Data.templateTopic = templateTopic;
            injectTopics(Data);
        }
        else {
            throw "ShowTopics: data Invalid"
        }
    });
});
function injectTopics(Data) {
    for (let topic of Data.Topics) {
        if (topic.ArticlesInfo.Count > 0)
            Data.date = new Date(topic.ArticlesInfo.CreatedDate).toLocaleString(Data.Locale);
        let HTML = "";
        HTML = Data.templateTopic
            .replaceAll("{{TopicID}}", topic.Id)
            .replaceAll("{{TopicHref}}", `/${Data.Locale}/Forum/Topic/${topic.Id}`)
            .replaceAll("{{TopicTitle}}", topic.Title)
            .replaceAll("{{TopicDescrtiption}}", topic.Descrtiption)
            .replaceAll("{{ArticleCount}}", topic.ArticlesInfo.Count)
            .replaceAll("{{AuthorHref}}", 'none')
            .replaceAll("{{RealName}}", topic.Author.RealName)
            .replaceAll("{{LastArtDate}}", Data.date)
            .replaceAll("{{SenderHref}}", 'none')
            .replaceAll("{{SenderName}}", topic.ArticlesInfo.RealName);
        Data.tbody.innerHTML += HTML;
    }
};
function getAll(Path) {
    return Promise.all([GetTopics(), GetHTML(Path)])
}