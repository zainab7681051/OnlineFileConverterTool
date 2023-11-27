<template>
    <div class="upload-file">
        <form @submit.prevent="uploadFile">
            <label for="fileInput" class="file-label">
                <span>Choose a file</span>
                <input type="file" id="fileInput" ref="fileInput" @change="handleFileChange" />
            </label>

            <div v-if="selectedFile" class="file-info">
                <p>Selected File: {{ selectedFile.name }}</p>
                <p>File Size: {{ formatFileSize(selectedFile.size) }}</p>
            </div>

            <button type="submit" :disabled="!selectedFile">Upload File</button>
        </form>
    </div>
</template>

<script lang="ts">
import { Convert } from "../api/converterApi.ts"
export default {
    data() {
        return {
            selectedFile: null as File | null
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

            return `${size.toFixed(2)} ${units[index]}`;
        },

        async uploadFile() {
            if (this.selectedFile) {
                try {
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
                                    return "inavlid file type"
                                }
                            } else {
                                return lastPart;
                            }
                        } else {
                            return 'inavlid file type';
                        }
                    }
                    let from = fileExt(f.name).toString();
                    console.log(`file etx is : ${from}`)
                    // let to = "docx";
                    // let res = await Convert(f, from, to);
                    // console.log("response: ", res);

                } catch (error) {
                    console.error('Error uploading file:', error);
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
