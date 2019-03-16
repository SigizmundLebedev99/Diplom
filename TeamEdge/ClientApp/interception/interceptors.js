import axios from 'axios';
import store from '../store/index'
import router from '../router/index'
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
  axios.interceptors.response.use(undefined,
    function (err) {
    if (err.response.status === 401) {
      store.dispatch('auth/reSign', router.path);
    }
    throw err;
  });
}

