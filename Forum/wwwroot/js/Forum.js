document.addEventListener("DOMContentLoaded", () => {
    //const app = document.getElementById("app");
    //if (!app) throw "Forum script: APP not found";
    //else {
    //const [html] = document.getElementsByTagName("html")
    //const Locale = html.getAttribute("lang");

    let tbody = document.getElementById("Forum").getElementsByTagName("tbody")[0];
    loadTopics(tbody);
    /*}*/
});
function loadTopics(tbody) {
    fetch("/api/Topic")
        .then(r => r.json())
        .then(j => {
            console.log(j.length);
            console.log(j);
            if (j instanceof Array) {
                showTopics(tbody, j);
            }
            else {
                throw "ShowTopics: data Invalid"
            }
        });
};
function showTopics(tbody, j) {
    for (let topic of j) {
        fetch(`/api/User/${topic.authorId}`)
            .then(r => r.json())
            .then(j => {
                tbody.innerHTML += `<tr data-id='${topic.id}'>
                    <th></th>
                    <td id="topic_Field"><a href="#" id="topic_Href"><b>${topic.title}</b><br/><i>${topic.descrtiption}</i><a></td>
                    <td> ? </td>
                    <td id="topic_AuthorName"><a href="#" id="Author_Href">${j.realName}</a></td>
                    <td> ? </td></tr>`;
                document.getElementById("topic_Href").addEventListener('click',()=> updateImageDisplay(topic.title));
            });
        
    }
}
function updateImageDisplay(name) {
    console.log(name);
}