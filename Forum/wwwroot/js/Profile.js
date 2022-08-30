import { ShowInfo, SAvatarUpload, SAvatarChange, validFileType } from '/js/Function.js';
import { GetHTML, GetArticles } from '/js/FetchRequest.js';
document.addEventListener('DOMContentLoaded', () => {
    const [html] = document.getElementsByTagName("html")
    const Locale = html.getAttribute("lang");
    const ProfileConteiner = document.querySelector('.ProfileConteiner');
    const UserId = ProfileConteiner.getAttribute("id");
    const DATableBTN = document.querySelector('.DelTableBTN');
    const Param = "?ParamSearch=Deleted&GuidType=User";
    const tbody = document.querySelector(".DeleteArticleTable tbody");
    console.log(tbody);
    getAll(UserId, Param, '/templates/DeleteArticle.html',).then(([Articles, temlateDA]) => {
        for (let Article of Articles) {
            let _Date = '';
            let now = new Date();
            let ArticleDate = new Date(Article.status.operationDate);
            if (now.toLocaleDateString('en-US') === ArticleDate.toLocaleDateString('en-US')) {
                _Date =`${ArticleDate.getHours()}:${ArticleDate.getMinutes()}`;
            }
            else {
                _Date = ArticleDate.toLocaleString(Locale);
            }
            let html = temlateDA
                .replaceAll("{{ArticleDeletedId}}", Article.id)
                .replaceAll("{{ArticleDeleteDate}}", _Date)
                .replaceAll("{{TopicTitle}}", Article.topic.title)
                .replaceAll("{{TopicDescrtiption}}", Article.topic.descrtiption)
                .replaceAll("{{ArticleText}}", Article.text);
            tbody.innerHTML += html;
        }
        console.log(Articles, temlateDA);
        DATableBTN.onclick = HiddeTable;
    })
    //
    //info hidde
    //
    const Info = document.getElementsByClassName('info')[0];
    if (Info != null) {
        Info.style.marginTop  = (Info.offsetHeight / 2) * -1 + 'px';
        Info.style.marginLeft = (Info.offsetWidth / 2) * -1 + 'px';
        setTimeout(e => {
            Info.style.display = "none";
        }, 1500);
    }
    //
    //password form 
    //
    const PassForm = document.getElementById('PassForm');
    const PassLink = document.getElementById('PassLink');
    const Linkhref = document.getElementById('PassLink_href');
    document.getElementById('hidde').onclick = e => {
        PassForm.style.display = "none";
        PassLink.style.display = "inline";
    }
    if (PassLink !== null)
        Linkhref.onclick = e => {
            PassForm.style.display = "inline";
            PassLink.style.display = "none";
        }
    //
    //user avatar logic
    //
    const UploadAvaTxt = document.querySelector("Name1").innerText;
    const CnahgeAvaTxt = document.querySelector("Name2").innerText;
    const ButtonAvaTxt = document.querySelector("Text");
    const pic          = document.getElementById('AvatarIMG');
    const ava          = document.getElementById('avatar_Select_Button');
    const Error        = document.getElementById('avaError');
    const ButtonAvatar = document.getElementById('ButtonAvatar');
    ButtonAvaTxt.innerText = UploadAvaTxt;
    ava.addEventListener('change', updateImageDisplay);
    function updateImageDisplay() {
        var curFile = ava.files[0];
        if (ava.files.length === 0) {
            Error.style.display = "inline";
            Error.textContent = 'No files currently selected for upload';
        }
        else {
            if (validFileType(curFile)) {
                let saveSRC = pic.src;
                pic.src = window.URL.createObjectURL(curFile);
                const formData = new FormData();
                formData.append("Avatar", curFile);
                ButtonAvaTxt.innerText = CnahgeAvaTxt;
                ButtonAvatar.onclick = e => {
                    fetch(`/${Locale}/Auth/ChangeAvatar`, {
                        method: "POST",
                        body: formData
                    }).then(t => t.text())
                        .then(t => {
                            if (t !== 'Success') {
                                pic.src = saveSRC;
                                Error.style.display = "inline";
                                Error.textContent = t;
                            }
                            saveSRC = null;
                            document.getElementById('AvatarInNav').src = pic.src;
                            ShowInfo(t, ProfileConteiner);
                            SAvatarUpload(ButtonAvatar);
                            ButtonAvaTxt.innerText = UploadAvaTxt;
                            ButtonAvatar.onclick = e => {
                                ava.click();
                            }
                        })
                }
            }
            else {
                Error.style.display = "inline";
                Error.textContent = 'Not a valid file type. ';
            }
        }
    }
    //
    //User Data change
    //
    for (let i = 1; i <= 3; i++) {
        let userData;
        let Path;
        if (i === 1) {
            userData = document.getElementById("RealName");
            Path = `/${Locale}/Auth/ChangeRealName?NewName=`;
        }
        if (i === 2) {
            userData = document.getElementById("Email");
            Path = `/${Locale}/Auth/ChangeEmail?Email=`;
        }
        if (i === 3) {
            userData = document.getElementById("Login");
            Path = `/${Locale}/Auth/ChangeLogin?Login=`;
        }
        userData.onclick = e => {
            e.target.setAttribute("contenteditable", true);
            e.target.savedValue = e.target.innerText;
        };
        userData.onkeydown = e => {
            if (e.code === 'Enter') {
                e.preventDefault();
                userData.blur();
            }
        }
        userData.onblur = e => {
            e.target.removeAttribute("contenteditable");
            if (e.target.savedValue != e.target.innerText) {
                fetch(Path + e.target.innerText)
                    .then(r => r.text())
                    .then(t => {
                        if (t === 'Error') {
                            ShowInfo(t, ProfileConteiner);
                            e.target.innerText = e.target.savedValue;
                        }
                        else {
                            ShowInfo(t, ProfileConteiner);
                        }
                    })
            }
        };
    }
});
function HiddeTable(e) {
    const DATable = document.querySelector(".DeleteArticleTable");
    if (DATable.style.display === "none")
        DATable.style.display = "block";
    else
        DATable.style.display = "none";
}
function getAll(UserID, Param, Path_DA) {
    return Promise.all([
        GetArticles(UserID, Param),
        GetHTML(Path_DA),
    ]);
}