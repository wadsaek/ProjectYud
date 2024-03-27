


function validateSong() {
    let inp = document.forms["SongInputForm"]["songInput"].value;
    let ind = inp.indexOf("/track")
    if ((ind == -1) || !(inp.includes("?si"))) {
        console.log("invalid song");
        alert("are you sure you entered a valid spotify link?");
        return false;
    }
    else {
        console.log(ind)
        console.log(inp.slice(ind+7, ind+7+22))
        return true;
    }
}

function isStrongPassword(password) {
    // Define regex patterns for each criteria
    const uppercaseRegex = /[A-Z]/;
    const lowercaseRegex = /[a-z]/;
    const numberRegex = /\d/;
    const specialCharRegex = /[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]/;

    // Check if the password meets all criteria
    const hasUppercase = uppercaseRegex.test(password);
    const hasLowercase = lowercaseRegex.test(password);
    const hasNumber = numberRegex.test(password);
    const hasSpecialChar = specialCharRegex.test(password);
    const isLengthValid = password.length >= 8;

    // Return true if all criteria are met, otherwise return false
    return hasUppercase && hasLowercase && hasNumber && hasSpecialChar && isLengthValid;
}

function validateRegistration() {
    let flag = true;
    const myForm = document.forms["registrationForm"];
    if (!isStrongPassword(myForm["passwordInput"].value)) {
        alert("ew bad password not tasty");
        flag = false;
    }
    if (myForm["passwordInput"].value != myForm["passwordInputConfirm"].value) {
        alert("passwords don't match");
        flag = false;
    }
    console.log("what");
    return flag;
}
