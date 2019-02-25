const mutations = {
  setOpened(state, payload){
    state.open = payload;
  },
  signedIn(state, payload){
    state.profile = payload;
  }
}

const getters = {
  getOpened:state=>state.open,
  isLoggedId : state => !!state.profile,
  token : state => state.profile.token,
}

const state = ()=>({
  open: false,
  isLoggingIn:false,
  profile: {
    Id:null,
    firstName:null,
    lastName:null,
    patronymic:null,
    email:null,
    avatar:null
  }
})

export default {
  namespaced: true,
  mutations,
  getters,
  state,
}
