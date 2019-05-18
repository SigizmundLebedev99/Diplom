import Vue from 'vue'
import Vuex from 'vuex'
import auth from './auth'
import projects from './projects'
import project from './project'
import profile from './profile'
import createWorkItem from './work-item/create-work-item'
import fileSelector from './work-item/file-selector'
import backlog from './backlog'
import timesheet from './timesheet'
import message from './message'
Vue.use(Vuex)

export default new Vuex.Store({
modules:{
  auth,
  projects,
  project,
  createWorkItem,
  fileSelector,
  profile,
  backlog,
  timesheet,
  message
}
})
