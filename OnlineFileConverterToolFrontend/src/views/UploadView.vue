<template>
    <h1>Upload a file here.</h1>
    <h2>max size: 50 MB</h2>
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
h1 {
    align-self: center;
}

h2 {
    align-self: center;
}
</style>
