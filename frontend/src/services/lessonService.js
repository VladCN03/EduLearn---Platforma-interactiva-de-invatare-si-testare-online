import { api } from './api';

export const getLessons = async () => {
    const response = await api.get('/Lessons');
    return response.data;
};