const baseUrl: string = "https://fileconverttool01.bsite.net/api/v1/Converter";
const tag: string = "Upload";

export async function Convert(file: File, fromFormat: string, toFormat: string): Promise<Response | null> {
    try {
        console.log(baseUrl)
        const formData = new FormData();
        formData.append('file', file)

        const requestOptions: RequestInit = {
            method: 'POST',
            body: formData,
            mode: 'cors',
        };
        const param = "from=" + fromFormat + "&to=" + toFormat;
        const url = baseUrl + '/' + tag + '?' + param;
        const res = await fetch(url, requestOptions);
        return res;
    } catch (error) {
        return null;
    }
};