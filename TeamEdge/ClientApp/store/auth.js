import router from '../router/index'

const mutations = {
  setOpened(state, payload){
    state.returnUrl = null;
    state.open = payload;
  },
  setReturnUrl(state, payload){
    state.returnUrl = payload;
  },
  signedIn(state) {
    var user = localStorage.getItem('user');
    if(user)
      state.profile = (JSON.parse(user))
    else
      state.profile = null;
  },
  signIn(state, payload){
    state.profile = payload.profile;
    if(payload.remember)
    {
      localStorage.setItem('user', JSON.stringify(payload.profile));
    }
    state.open = false;
    if(state.returnUrl){
      router.push(returnUrl);
      state.returnUrl = null;
    }
  },
  signOut(state){
    state.profile = null;
    router.push('/');
    localStorage.removeItem('user');
  }
}

const actions = {
  signedIn({commit, dispatch, getters}){
    commit('signedIn');
    if(getters.profile){
      router.push({name:'projects'});
      dispatch('projects/fetchProjects',{},{root:true});
    }
  },
  signIn({commit, dispatch}, profile){
    commit('signIn', profile);
    router.push({name:'projects'});
    dispatch('projects/fetchProjects',{},{root:true});
  },
  reSign({commit}, returnUrl){
    commit('signOut');
    commit('setOpened', true);
    commit('setReturnUrl', returnUrl);
  }
}

const getters = {
  getOpened:state=>state.open,
  profile:state=>state.profile,
  token:state=>state.profile?state.profile.access_token:null
}

const state = ()=>({
  open: false,
  returnUrl:null,
  profile: {
    Id:null,
    firstName:null,
    lastName:null,
    patronymic:null,
    email: null,
    avatar: null,
    access_token:null
  }
})

export default {
  namespaced: true,
  mutations,
  getters,
  state,
  actions
}
