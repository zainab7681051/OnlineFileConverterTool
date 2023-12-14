const baseUrl: string = import.meta.env.VITE_BASE_URL;
const tag: string = "Upload";

export async function Convert(file: File, fromFormat: string, toFormat: string): Promise<Response | null> {
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
        return await fetch(url, requestOptions);
    } catch (error) {
        return null;
    }
};