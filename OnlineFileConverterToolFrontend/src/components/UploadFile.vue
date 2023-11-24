Copy code
<template>
    <div class="upload-file">
        <form @submit.prevent="uploadFile">
            <label for="fileInput" class="file-label">
                <span>Choose a file</span>
                <input type="file" id="fileInput" ref="fileInput" @change="handleFileChange"
                    accept=".jpg, .jpeg, .png, image/*" />
            </label>

            <div v-if="selectedFile" class="file-info">
                <p>Selected File: {{ selectedFile.name }}</p>
                <p>File Size: {{ formatFileSize(selectedFile.size) }}</p>
            </div>

            <button type="submit" :disabled="!selectedFile">Upload File</button>
        </form>
    </div>
</template>

<script src="./UploadFile.ts"></script>

<style scoped>
.upload-file {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin: 20px;
}

.file-label {
    display: flex;
    flex-direction: column;
    align-items: center;
    cursor: pointer;
    padding: 10px;
    border: 2px dashed #ccc;
    border-radius: 5px;
    transition: border-color 0.3s ease-in-out;
}

.file-label:hover {
    border-color: #4caf50;
}

.file-info {
    margin-top: 10px;
    text-align: center;
    font-size: 14px;
    color: #333;
}
</style>
