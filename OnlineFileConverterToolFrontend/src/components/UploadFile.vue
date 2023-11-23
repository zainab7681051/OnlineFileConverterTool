<template>
    <div class="upload-file">
        <label for="fileInput" class="file-label">
            <span>Choose a file</span>
            <input type="file" id="fileInput" ref="fileInput" @change="handleFileChange" accept="*/*" />
        </label>

        <div v-if="selectedFile" class="file-info">
            <p>Selected File: {{ selectedFile.name }}</p>
            <p>File Size: {{ formatFileSize(selectedFile.size) }}</p>
        </div>
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
