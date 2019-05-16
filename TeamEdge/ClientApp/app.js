import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import Vuetify from 'vuetify'
import 'material-design-icons-iconfont/dist/material-design-icons.css'
import interceptionSetup from './interception/interceptors'
import CKEditor from '@ckeditor/ckeditor5-vue'
import getDate from './mixins/get-date' 
Vue.mixin(getDate);

Vue.use( CKEditor );
Vue.use(Vuetify, {
  theme: {
    primary: '#5271ff',
    secondary: '#8c9eff',
    accent: '#FFE27E',
    error: '#FF5252',
    info: '#2196F3',
    success: '#4CAF50',
    warning: '#FFC107'
  },
  iconfont: 'md'
})

Vue.prototype.$http = axios
sync(store, router)
interceptionSetup();
const app = new Vue({
  store,
  router,
  ...App
})

store.dispatch('auth/signedIn')

export {
  app,
  router,
  store
}
