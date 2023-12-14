import { defineString } from 'firebase-functions/params';

const BASE_URL = defineString('BASE_URL');
const baseUrl: string = BASE_URL.value();
const tag: string = "Upload";

export async function Convert(file: File, fromFormat: string, toFormat: string): Promise<Response> {
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
};