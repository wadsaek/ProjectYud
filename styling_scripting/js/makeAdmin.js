function makeAdmin(element,victimId) {
    if (!checkNumerity(victimId)) {
        return;
    }
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        switch (this.responseText) {
            case "success":
                element.remove();
                alert("done!")
                break;
            case "notAnAdmin":
                alert("you aren't an admin")
                break;
            default:
                alert(this.responseText)
                break;
        }
    }
    xhttp.open("GET", `../aspx/processer.aspx?action=makeAnAdmin&victim=${victimId}`)
    xhttp.send();
}