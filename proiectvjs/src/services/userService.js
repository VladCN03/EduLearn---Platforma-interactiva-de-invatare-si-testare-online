import { api } from './api';

export const getUsers = async () => {
    const response = await api.get('/Users');
    return response.data;
};

export const createUser = async (user) => {
    const response = await api.post('/Users', user);
    return response.data;
};