async function GetHTML(Path) {
    const response = await fetch(Path);
    return await response.text();
}
async function GetArticles(Id, Param = "") {
    console.log(Param);
        const response = await fetch(`/api/Article/${Id}` + Param);
    return await response.json();
}
async function DeleteArticle(ArticleId, UserID) {
    const response = await fetch(`/api/Article/${ArticleId}`, {
        method: 'DELETE',
        headers: {
            'UserID': `${UserID}`
        }
    });
    return await response.json();
};
async function RestoreArticle(UserId, ArticleId) {
    const response = await fetch(`/api/Article/?id=${UserId}&ArticleId=${ArticleId}`, {
        method: 'RESTORE'
    });
    return await response.json();
}
async function GetUser(Id) {
    const response = await fetch(`/api/User/${Id}`);
    return await response.json();
}
async function GetTopics() {
    const response = await fetch("/api/Topic");
    return await response.json();
};
export { GetHTML, GetArticles, GetUser, GetTopics, DeleteArticle, RestoreArticle};