import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import process from 'process';

const certFilePath = path.join(process.env.APPDATA, 'ASP.NET/https', 'reactapp1.client.pem');
const keyFilePath = path.join(process.env.APPDATA, 'ASP.NET/https', 'reactapp1.client.key');

export default defineConfig({
    plugins: [plugin()],
    server: {
        https: {
            key: fs.readFileSync(keyFilePath),
            cert: fs.readFileSync(certFilePath),
        },
        proxy: {
            '/api': {
                target: 'https://localhost:7004', // ???ng d?n ??n API c?a b?n
                secure: false,  // T?t ki?m tra ch?ng ch? t? ký
            },
        },
        port: 5173,
    },
});
