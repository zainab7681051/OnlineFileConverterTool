<template>
    <div class="upload-file">
        <form @submit.prevent="uploadFile">
            <label for="fileInput" class="file-label">
                <span>Choose a file</span>
                <input type="file" id="fileInput" ref="fileInput" @change="handleFileChange" />
            </label>

            <div v-if="selectedFile" id="file-info" class="file-info">
                <p>{{ selectedFile.name }}</p>
                <p>{{ formatFileSize(selectedFile.size) }}</p>
                <p id="FileLimit" :class="FileSizeExceedsLimit ? 'shown error' : 'hidden'">FILE SIZE EXCEEDS LIMIT!!!</p>
            </div>
            <div class="searchable-select">
                <label for="searchSelect">Choose Format:</label>

                <input v-model="searchQuery" @input="filterOptions" type="text" id="searchSelect"
                    placeholder="Search or select a format" autocomplete="off" />

                <select v-model="selectedFormat" id="selectOptions">
                    <option>{{ nullVal }}</option>
                    <option v-for="(format, index) in filteredOptions" :key="index" :value="format">{{ format }}</option>
                </select>

            </div>
            <button id="convertBtn" class="btn1" type="submit"
                :disabled="(!selectedFile || !selectedFormat) || FileSizeExceedsLimit" @click="uploadFile">
                Convert File
            </button>
        </form>
        <div id="error" class="error hidden"><span id="errorText"></span><button class="btn1" @click="">close</button>
        </div>
        <button id="downloadBtn" class="btn1 hidden">Download File</button>
    </div>
</template>

<script lang="ts">
import { Convert } from "../api/converterApi.ts"
import { allFormats } from "../api/allFormats.ts"
type response = Response | {
    ok: false;
    status: number;
    json: () => void;
}
export default {
    data() {
        return {
            selectedFile: null as File | null,
            result: null as any,
            FileSizeExceedsLimit: false as boolean,
            selectedFormat: null as string,
            supportedFormats: allFormats as string[],
            searchQuery: "" as string,
            FilteredFormats: null as string[],
            nullVal: null as null
        };
    },
    components: {},
    computed: {
        filteredOptions() {
            this.FilteredFormats = this.supportedFormats.filter(
                (format: string) =>
                    format.toLowerCase().includes(this.searchQuery.toLowerCase())
            );
            return this.FilteredFormats
        },
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
                        this.FileSizeExceedsLimit = true;
                        break;

                    case (size > 50 || size < 50) && index > 2:
                        this.FileSizeExceedsLimit = true;
                        break;

                    default:
                        this.FileSizeExceedsLimit = false;
                        break;
                }
            } else { this.FileSizeExceedsLimit = false; }
            let finalSize = size.toFixed(2);
            return `${finalSize} ${units[index]}`;
        },
        async uploadFile() {
            const convertBtn = document.getElementById("convertBtn");
            const downloadBtn = document.getElementById("downloadBtn");
            const ErrorBox = document.getElementById("error");
            const errorText = document.getElementById("errorText");
            const dialogButton = ErrorBox.querySelector('button');
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
                let from: string = fileExt(f.name).toString();
                let to: string = this.selectedFormat;
                let res: response = await Convert(f, from, to) || { ok: false, status: 900, json: () => { } };
                this.result = res.status != 900 ? await res.json() : { error: { message: "Failed to fetch. Possible network failure" } };
                if (!res.ok) {
                    console.error('Error uploading file:', this.result.error);
                    ErrorBox.classList.remove("hidden");
                    ErrorBox.classList.add("shown")
                    errorText.innerText = `Error Code ${res.status} : ${this.result.error.message}`;
                    dialogButton.addEventListener("click", () => {
                        ErrorBox.classList.remove("shown")
                        ErrorBox.classList.add("hidden");
                        errorText.innerText = "";
                    })

                } else {
                    ErrorBox.classList.remove("shown")
                    ErrorBox.classList.add("hidden");
                    downloadBtn.classList.remove("hidden");
                    downloadBtn.classList.add("shown");
                    downloadBtn.addEventListener('click', () => {
                        window.open(this.result.url, '_blank')
                    });
                }
            }
        },
        filterOptions() {
            this.selectedFormat = this.searchQuery ? this.FilteredFormats[0] : this.nullVal;
        }
    },

    mounted() {
    },
};
</script>

<style scoped>
@import url('./UploadFile.css')
</style>
