export default {
    data() {
        return {
            selectedFile: null as File | null,
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
                const formData = new FormData();
                formData.append('file', this.selectedFile);

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
        },
    },

    mounted() {
        console.log('Component is mounted');
        // Additional logic on mount
    },
};