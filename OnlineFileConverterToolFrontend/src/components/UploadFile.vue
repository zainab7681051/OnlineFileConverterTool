<template>
    <div class="upload-file">
        <form @submit.prevent="uploadFile">
            <label for="fileInput" class="file-label">
                <span>Choose a file</span>
                <input type="file" id="fileInput" ref="fileInput" @change="handleFileChange" />
            </label>

            <div v-if="selectedFile" id="file-info" class="file-info">
                <p>Selected File: {{ selectedFile.name }}</p>
                <p>File Size: {{ formatFileSize(selectedFile.size) }}</p>
                <p id="FileLimit" :class="FileSizeExceedsLimit ? 'shown error' : 'hidden'">FILE SIZE EXCEEDS LIMIT!!!</p>
            </div>

            <button id="convertBtn" class="btn1" type="submit" :disabled="!selectedFile || FileSizeExceedsLimit">Convert
                File</button>
        </form>
        <label id="error" class="error hidden"></label>
        <button id="downloadBtn" class="btn1 hidden">Download File</button>
    </div>
</template>

<script lang="ts">
import { Convert } from "../api/converterApi.ts"
export default {
    data() {
        return {
            selectedFile: null as File | null,
            result: null as any,
            FileSizeExceedsLimit: false as boolean
        };
    },
    methods: {
        handleFileChange(event: Event) {
            const input = event.target as HTMLInputElement;
            const file = input.files?.[0] || null;

            if (file) {
                this.selectedFile = file;
            }
        },

        formatFileSize(size: number) {
            const units = ['B', 'KB', 'MB', 'GB', 'TB'];
            let index = 0;

            while (size >= 1024 && index < units.length - 1) {
                size /= 1024;
                index++;
            }

            if (index >= 2) {
                switch (true) {
                    case size > 50 && index === 2:
                        console.log("here1");
                        this.FileSizeExceedsLimit = true;
                        break;

                    case (size > 50 || size < 50) && index > 2:
                        console.log("here2");
                        this.FileSizeExceedsLimit = true;
                        break;

                    default:
                        this.FileSizeExceedsLimit = false;
                        break;
                }
            } else { this.FileSizeExceedsLimit = false }
            let finalSize = size.toFixed(2);
            return `${finalSize} ${units[index]}`;
        },
        async uploadFile() {
            var downloadBtn = document.getElementById("downloadBtn");
            var errorMessage = document.getElementById("error");
            if (this.selectedFile) {
                let f: File = this.selectedFile;
                const fileExt = (filename: string): string => {
                    const parts: string[] = filename.split('.');
                    let l: number = parts.length;
                    if (l > 1) {
                        const lastPart: string = parts.pop().toLowerCase();

                        if (l > 2) {
                            const secondToLastPart: string = parts.pop();
                            if (secondToLastPart === "tar") {
                                const TheParts: string[] = [secondToLastPart, lastPart];
                                return `${TheParts.join('.')}`;
                            } else {
                                return lastPart;
                            }
                        } else {
                            return lastPart;
                        }
                    } else {
                        return 'inavlid file type';
                    }
                }
                let from = fileExt(f.name).toString();
                let to = "docx";
                let res = await Convert(f, from, to);
                this.result = await res.json();
                if (!res.ok) {
                    console.error('Error uploading file:', this.result);
                    errorMessage.classList.remove("hidden");
                    errorMessage.classList.add("shown")
                    errorMessage.innerText = `Error Code ${res.status} : ${this.result.error.message}`;
                } else {
                    errorMessage.classList.remove("shown")
                    errorMessage.classList.add("hidden");
                    downloadBtn.classList.remove("hidden");
                    downloadBtn.classList.add("shown");
                    downloadBtn.addEventListener('click', () => {
                        console.log(this.result)
                        window.open(this.result.url, '_blank')
                    });
                }
            }
        }
    },

    mounted() {
    },
};
</script>

<style scoped>
.upload-file {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin: 20px;
}

form {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-bottom: 20px;
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
    border-color: var(--borderGreen);
}

.file-info {
    margin-top: 10px;
    text-align: center;
    font-size: 14px;
    color: #333;
}

input#fileInput {
    scale: 0;
}

.btn1 {
    margin-top: 25px;
    width: 150px;
    font-size: 21px;
    border-radius: 8px;
    height: 35px;
    cursor: pointer;
    transition: border-color 0.3s ease-in-out;
}

.btn1:hover:not(:disabled) {
    background: var(--borderGreen);
}
</style>
