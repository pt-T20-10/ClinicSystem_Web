import axios from 'axios';

const instance = axios.create({

    baseURL: 'http://localhost:5156/api', 
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
    }
});


instance.interceptors.response.use(
    (response) => {
        // Chỉ lấy phần data của response
        return response.data;
    },
    (error) => {
        return Promise.reject(error);
    }
);

export default instance;