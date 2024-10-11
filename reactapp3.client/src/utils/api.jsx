// reactlogin.client/src/utils/api.js
import axios from 'axios';

const API_URL = "https://localhost:7004/api";  // Địa chỉ API ASP.NET Core

export const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

export default api;
