const baseUrl: string = "http://localhost:5111/api/v1/Converter"
const tag: string = "Upload";
async function Convert(file: File, fromFormat: string, toFormat: string): Promise<any> {
    try {
        const formData = new FormData();
        formData.append('from', fromFormat);
        formData.append('to', toFormat);
        formData.append('file', file)

        const requestOptions: RequestInit = {
            method: 'POST',
            headers: {
                'Accept': '*/*',
            },
            body: formData,
        };
        const res = await fetchData(baseUrl + '/' + tag, requestOptions);
        return res;
    } catch (error) {
        console.error(error);
    }
};

async function fetchData(apiUrl: string, requestOptions: RequestInit): Promise<any> {
    try {
        const response = await fetch(apiUrl, requestOptions);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const data = await response.json();
        return data;
    } catch (error) {
        // Handle errors
        console.error('Error:', error);
    }
}

export { Convert }