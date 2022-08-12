//import { validFileType } from '/js/Function.js';
document.addEventListener('DOMContentLoaded', () => {
    //
    //avatar select
    //
    let box = document.getElementById('tmp');
    let pic = document.getElementById('AvatarIMG');
    let ava = document.getElementById('avatar_Select_Button');
    let Error = document.getElementById('avaError');
    ava.addEventListener('change', updateImageDisplay);
    function updateImageDisplay() {
        var curFiles = ava.files[0];
        if (curFiles.length === 0) {
            Error.style.display = "inline";
            Error.textContent = 'No files currently selected for upload';
        }
        else
            if (validFileType(curFiles)) {
                box.style.display = "inline";
                pic.src = window.URL.createObjectURL(curFiles);
            }
            else {
                Error.style.display = "inline";
                Error.textContent = 'Not a valid file type. ';
            }
    }
    let fileTypes = ['image/jpeg', 'image/pjpeg', 'image/png']
    function validFileType(file) {
        for (let i = 0; i < fileTypes.length; i++) {
            if (file.type === fileTypes[i])
                return true;
        }
        return false;
    }
})