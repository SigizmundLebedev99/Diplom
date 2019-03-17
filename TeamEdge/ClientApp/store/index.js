import Vue from 'vue'
import Vuex from 'vuex'
import auth from './auth'
import projects from './projects'
import project from './project'
import createWorkItem from './work-item/create-work-item'
Vue.use(Vuex)

export default new Vuex.Store({
modules:{
  auth,
  projects,
  project,
  createWorkItem
}
})
