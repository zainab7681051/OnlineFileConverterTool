<template>
    <h1>Upload a file here</h1>
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
</style>
