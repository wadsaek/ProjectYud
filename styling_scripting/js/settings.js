function showDeletion() {
    document.getElementById("confirmation").style.display = "flex";
}

document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.querySelector('.fileupload');
    fileInput.addEventListener('change', function () {
        const file = fileInput.files[0]; // Get the selected file
        const fileName = file.name;
        const fileSize = file.size; // File size in bytes
        const maxFileSize = 4 * 1024 * 1024; // 4MiB in bytes

        const allowedExtensions = ['.jpg', '.jpeg', '.png'];
        const fileExtension = fileName.substring(fileName.lastIndexOf('.')).toLowerCase();

        if (fileSize > maxFileSize) {
            alert('wow this is way too much. try limiting yourself to 4MiB');
            // Clear the file input (optional)
            fileInput.value = '';
        } else if (allowedExtensions.indexOf(fileExtension) === -1) {
            alert('That ain\'t no image who ya fooling');
            // Clear the file input (optional)
            fileInput.value = '';
        }
    });
});