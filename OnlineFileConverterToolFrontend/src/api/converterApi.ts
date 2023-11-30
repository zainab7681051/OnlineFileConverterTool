const baseUrl: string = "http://localhost:5111/api/v1/Converter"
const tag: string = "Upload";

async function Convert(file: File, fromFormat: string, toFormat: string): Promise<Response> {
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
        return await fetch(url, requestOptions);;
    } catch (error) {
        console.error(error);
    }
};

export { Convert }