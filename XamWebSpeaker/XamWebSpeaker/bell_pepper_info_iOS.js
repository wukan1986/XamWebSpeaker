; /* for ignore BOM */

function bell_pepper_info_Log(text) {
    console.log(text);
    window.webkit.messageHandlers.Log.postMessage(text);
}

function bell_pepper_info_ReadPhrase() {
    /* return bell_pepper_info.ReadPhrase(); */
    return 1;
}

function bell_pepper_info_Speak(text) {
    window.webkit.messageHandlers.Speak.postMessage(text);
}

function bell_pepper_info_SpeakEnd(text) {
    window.webkit.messageHandlers.SpeakEnd.postMessage(text);
}

function bell_pepper_info_SplitSent(str) {
    /* return bell_pepper_info.SplitSent(str); */
    return str;
}

function bell_pepper_info_FileSaveAs(str) {
    /* return bell_pepper_info.FileSaveAs(str); */
}

function bell_pepper_info_MaxTextLen() {
    /* return bell_pepper_info.MaxTextLen(); */
    return 500;
}
