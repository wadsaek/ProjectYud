let chosenbutton = null;

function checkNumerity(number) {
    number -= 0;
    if (number == NaN) {

        console.log(number + "not numeric");
        reportError("wtf")
        alert("you aren't playing by the rules do you know that?")
        return false;
    }
    return true;
}
function deletecomment(a) {
    if (!checkNumerity(a)) {
        return;
    }
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        if (this.responseText == "success") {
            document.getElementById(`comment${a}`).remove();
        }
        else (
            console.log(this.responseText)
        )
    }
    xhttp.open("GET", `../aspx/processer.aspx?action=deletecomment&comment=${a}`)
    xhttp.send();

}

function addComment(PostId) {
    const CommId = document.getElementById("commId").value;
    const CommText = document.getElementById("newComment").value;

    if (!checkNumerity(PostId)) {
        return;
    }
    if (!checkNumerity(CommId)) {
        return;
    }
    const xhttp = new XMLHttpRequest();

    xhttp.onload = function(){
        const commentBlock = document.getElementById("commentaryBlock");
        const response = JSON.parse(this.responseText);
        const newCommArea = document.getElementById("newComment");
        switch(response[0]) {
            case"added":
                const username = response[1].trim();
                const newCommentId = response[2];
                const userid = response[3];
                const pfp = document.getElementById("UserPfp").src;
                const newComment = document.createElement("div");
                const NOW = new Date();
                newComment.id = `comment${newCommentId}`;
                newComment.className = "comment";
                newComment.innerHTML = `<div class="commentUserData">
                        <img class="pfp-image" src="${pfp}" height="22" width="22" title="${username}'s pfp" />
                        <a style="padding:inherit;" href="../aspx/Account.aspx?id=${userid}" ><b style="padding: inherit;">${username}</b></a>
                    </div>
                    <div class="textanddelte">
                        <div class="commentText" id="commenttext${newCommentId}">
                            ${CommText}
                        </div>
                        <div class="delete">
                            <img src="../images/uiElements/bin.png" title="delete comment" onclick="deletecomment(${newCommentId})" style="display:block" height="24" />
                            <img src="../images/uiElements/edit.png" title="edit comment" onclick="editcomment(${newCommentId})" style="display:block" height="24" />
                        </div>
                    </div>
                    <i title="${NOW.toLocaleString().replace(",","")}">${NOW.toLocaleTimeString().substring(0, 5)}</i>`
                commentBlock.prepend(newComment);
                newCommArea.value = "";
                break;
            case "edited":
                document.getElementById(`commenttext${CommId}`).innerHTML = CommText;
                const editId = document.getElementById("commId");
                newCommArea.value = "";
                editId.setAttribute("value", -1);
                break;
            default:
                console.log(`oopsies\n ${response[0]}`);
                document.getElementById("errorMessage").innerHTML = response[0];
                break;

        }
    }
    xhttp.open("GET", `../aspx/processer.aspx?action=addComment&CommText=${CommText}&CommId=${CommId}&SongId=${PostId}`);
    xhttp.send();
}

function editcomment(id) {
    if (!checkNumerity(id)) {
        console.log(id);
        return;
    }
    const oldComment = document.getElementById(`commenttext${id}`);
    const newCommArea = document.getElementById("newComment");
    const editId = document.getElementById("commId");
    newCommArea.value = oldComment.innerHTML.trim();
    editId.setAttribute("value", id);
}

function add_styles(postId) {
    if (!checkNumerity(postId)) {
        return;
    }
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        let reactiontype = this.responseText;
        if (this.responseText == "NoReaction") {
            return;
        }
        const reactButton = document.getElementById(`button${reactiontype}`);
        reactButton.className = "chosen-button"
        reactButton.onclick = function () { removeReaction(postId) };
        chosenbutton = reactButton;
    }

    xhttp.open("GET", `../aspx/processer.aspx?action=getreaction&song=${postId}`);
    xhttp.send();
}

function reaction(reactiontype, postId) {
    if (!checkNumerity(reactiontype)) {
        return;
    }
    if (!checkNumerity(postId)) {
        return;
    }
    
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        switch (this.responseText) {
            case "success":
                const numOfReacs = document.getElementById(`react${reactiontype}`)
                numOfReacs.innerHTML = Number(numOfReacs.innerHTML) + 1;
                const reactButton = document.getElementById(`button${reactiontype}`);
                reactButton.className = "chosen-button"
                reactButton.onclick = function () { removeReaction(postId) };
                chosenbutton = reactButton;
                break;
            case "the user has already put a reaction":
                alert("you have already put a reaction")
                break;
            case "something strange happened":
                alert(this.responseText)
            default:
                alert(`an error occured\n${this.responseText}`)
        }
    }
    xhttp.open("GET", `../aspx/processer.aspx?action=putreaction&song=${postId}&reactType=${reactiontype}`);
    xhttp.send();
}

function removeReaction(postId) {
    if (!checkNumerity(postId)) {
        return;
    }
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        if (this.responseText == "success") {
            const reactiontype = chosenbutton.id.slice(-1);
            const numOfReacs = document.getElementById(`react${reactiontype}`)
            numOfReacs.innerHTML = Number(numOfReacs.innerHTML) - 1;
            chosenbutton.className = "reaction-button"
            chosenbutton.onclick = function () { reaction(reactiontype, postId) };
            chosenbutton = null;
        }
        else {
            alert(this.responseText);
        }
    }

    xhttp.open("GET", `../aspx/processer.aspx?action=deleteReaction&song=${postId}`);
    xhttp.send();
}
