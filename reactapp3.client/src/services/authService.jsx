// reactlogin.client/src/services/authService.js
import api from '../utils/api';

export const register = async (data) => {
    return await api.post('/auth/register', data);
};

export const login = async (data) => {
    const response = await api.post('/auth/login', data);
    localStorage.setItem('token', response.data.token);  // Lưu token vào localStorage
    return response.data;
};

export const forgotPassword = async (email) => {
    return await api.post('/auth/forgot-password', { email });
};

export const resetPassword = async (data) => {
    return await api.post('/auth/reset-password', data);
};

export const changePassword = async (data) => {
    return await api.post('/auth/change-password', data);
};
