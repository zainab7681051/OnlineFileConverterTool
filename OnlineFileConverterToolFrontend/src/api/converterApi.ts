const baseUrl: string = "http://localhost:5111/api/v1/Converter"
const tag: string = "Upload";

async function Convert(file: File, fromFormat: string, toFormat: string): Promise<any> {
    try {
        const formData = new FormData();
        formData.append('file', file)

        const requestOptions: RequestInit = {
            method: 'POST',
            body: formData,
            mode: 'cors',
        };
        const param = "from=" + fromFormat + "&to=" + toFormat;
        const url = baseUrl + '/' + tag + '?' + param;
        const res = await fetchData(url, requestOptions);
        return res;
    } catch (error) {
        console.error(error);
    }
};

async function fetchData(apiUrl: string, requestOptions: RequestInit): Promise<any> {
    try {
        const response = await fetch(apiUrl, requestOptions);

        if (!response.ok) {
            throw new Error(`Status: ${response.status} `);
        }
        const data = await response.json();
        return data;
    } catch (error) {
        // Handle errors
        console.error('Error:', error);
    }
}

export { Convert }