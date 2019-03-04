import axios from 'axios';
import {mapGetters} from 'vuex'

axios.interceptors.request.use(function(config) {
    const token = mapGetters({token:'auth.token'})[0];
    if(token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, function(err) {
    return Promise.reject(err);
});
