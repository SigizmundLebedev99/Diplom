const mutations = {
  setOpened(state, payload){
    state.open = payload;
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
  },
  signOut(state){
    state.profile = null;
    localStorage.removeItem('user');
  }
}

const getters = {
  getOpened:state=>state.open,
  isLoggedIn : state => state.profile,
  profile: state => state.profile,
  token:state=>state.profile?state.profile.token:null
}

const state = ()=>({
  open: false,
  profile: {
    Id:null,
    firstName:null,
    lastName:null,
    patronymic:null,
    email: null,
    avatar: null
  }
})

export default {
  namespaced: true,
  mutations,
  getters,
  state,
}
