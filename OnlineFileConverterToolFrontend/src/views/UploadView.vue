<template>
    <div class="heading">
        <h1>File Converter Tool</h1>
        <h2>Upload a file here.</h2>
        <h3>max size: <span style="color: var(--errorRed);">50 MB</span></h3>
    </div>
    <UploadFile />
</template>

<script lang="ts">
import UploadFile from '../components/UploadFile.vue'
export default {
    data() { return {} },
    components: { UploadFile }
}
</script>
<style scoped>
.heading {
    display: flex;
    flex-direction: column;
    align-items: center;
}

.heading>* {
    margin-block-end: .1em;
}

.heading>h1 {
    font-family: 'Rubik Mono One', monospace;
    color: var(--yellow)
}
</style>
