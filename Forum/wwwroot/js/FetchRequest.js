async function GetHTML(Path) {
    const response = await fetch(Path);
    return await response.text();
}
async function GetArticles(TopicId) {
    const response = await fetch(`/api/Article/${TopicId}`);
    return await response.json();
}
async function GetUser(UserId) {
    const response = await fetch(`/api/User/${UserId}`);
    return await response.json();
}
async function GetTopics(tbody) {
    const response = await fetch("/api/Topic");
    return await response.json();
};
export { GetHTML, GetArticles, GetUser, GetTopics };