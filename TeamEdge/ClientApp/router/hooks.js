import store from '../store/index'

export default {
  ifAuthenticated :
  (to, from, next) => {
    if (store.getters['auth/profile']) {
      next();
      return;
    }
    store.dispatch('auth/reSign', store.state.route.path);
  }
};
