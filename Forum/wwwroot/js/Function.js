function ShowInfo(status, ProfileConteiner) {
    let div = document.createElement('div');
    let b = document.createElement('b');
    div.setAttribute('class', 'info');
    if (status != 'Success') {
        div.id = 'infoError';
    }
    b.innerText = status;
    div.append(b);
    ProfileConteiner.append(div);
    div.style.marginTop = (div.offsetHeight / 2) * -1 + 'px';
    div.style.marginLeft = (div.offsetWidth / 2) * -1 + 'px';
    setTimeout(e => {
        ProfileConteiner.removeChild(div);
    }, 1500);
}
function SAvatarUpload(ButtonAvatar) {
    ButtonAvatar.style.color = 'rgb(49, 80, 135)';
    ButtonAvatar.style.background = 'rgba(46,57,77,0.4)';
    ButtonAvatar.style.boxShadow = '0 5px 10px rgba(84, 108, 96, 0.5)';
}
function SAvatarChange(ButtonAvatar) {
    ButtonAvatar.style.color = '#1b6b43';
    ButtonAvatar.style.background = 'rgba(60,80,89,0.5)';
    ButtonAvatar.style.boxShadow = '0 0 40px rgba(84, 108, 96, 0.8)';
}
let fileTypes = ['image/jpeg', 'image/pjpeg', 'image/png']
function validFileType(file) {
    for (let i = 0; i < fileTypes.length; i++) {
        if (file.type === fileTypes[i])
            return true;
    }
    return false;
}
export { ShowInfo, SAvatarUpload, SAvatarChange, validFileType };
