import axios from 'axios';
import store from '../store/index'

export default function(){
    axios.interceptors.request.use(function(config) {
        const token = store.getters['auth/token'];
        if(token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    }, function(err) {
        return Promise.reject(err);
    });
}

