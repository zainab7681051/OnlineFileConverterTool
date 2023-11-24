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

<script setup lang="ts">
import { ref } from 'vue';

const selectedFile = ref<File | null>(null);

const handleFileChange = (event: Event) => {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] || null;

    if (file) {
        selectedFile.value = file;
    }
};

const formatFileSize = (size: number) => {
    const units = ['B', 'KB', 'MB', 'GB', 'TB'];
    let index = 0;

    while (size >= 1024 && index < units.length - 1) {
        size /= 1024;
        index++;
    }

    return `${size.toFixed(2)} ${units[index]}`;
};

const uploadFile = async () => {
    if (selectedFile.value) {
        const formData = new FormData();
        formData.append('file', selectedFile.value);

        try {
            const response = await fetch('http://your-dotnet-api-endpoint', {
                method: 'POST',
                body: formData,
            });

            if (response.ok) {
                console.log('File uploaded successfully!');
                // Add any additional handling for a successful upload
            } else {
                console.error('Failed to upload file');
                // Handle error response from the server
            }
        } catch (error) {
            console.error('Error uploading file:', error);
        }
    }
};
</script>

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
