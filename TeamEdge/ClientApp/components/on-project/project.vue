<template>
    <div>
        <v-navigation-drawer
            dark
            v-model="drawer"
            :mini-variant.sync="mini"
            hide-overlay
            stateless
            app>
            <v-list class="pa-0" v-show="mini">
                <v-list-tile avatar>
                    <v-list-tile-avatar color="white">
                        <v-icon v-if="!project.logo" size="32" color="primary">work</v-icon>
                        <img v-else :src="project.logo"/>
                    </v-list-tile-avatar>
                </v-list-tile>
            </v-list>
            <v-container v-show="!mini" class="p-3">
            <v-layout row>
                <v-flex xs2></v-flex>
                <v-flex xs8>
                    <v-layout column align-center>
                        <v-avatar avatar color="white mt-2" size="80">
                            <v-icon v-if="!project.logo" size="60" color="primary">work</v-icon>
                            <img v-else :src="project.logo"/>
                        </v-avatar>
                        <span class="mt-3 title white--text">{{project.name}}</span>
                    </v-layout>
                </v-flex>
                <v-flex xs2>
                    <v-btn class="mt-0" icon flat>
                        <v-icon>more_vert</v-icon>
                    </v-btn>
                </v-flex>
            </v-layout>
            </v-container>
 
            <v-divider class="my-0"></v-divider>
            <v-list class="py-0">
                <v-list-tile :class="routeSel('project')"
                    @click.stop="goTo('project')">
                    <v-list-tile-action>
                        <v-icon class="trans" :class="routeSel('project')">dashboard</v-icon>
                    </v-list-tile-action>

                    <v-list-tile-content>
                    <v-list-tile-title>Доска</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
                <v-list-tile :class="routeSel('backlog')"
                    @click.stop="goTo('backlog')">
                    <v-list-tile-action>
                        <v-icon class="trans" :class="routeSel('backlog')">build</v-icon>
                    </v-list-tile-action>

                    <v-list-tile-content>
                    <v-list-tile-title>Единицы работы</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
            </v-list>
            <v-divider class="my-0"></v-divider>
            <v-list class="py-0 px-0">
                <v-list-tile v-for="(wi,i) in workItems" :key="i"
                        class="px-0 my-0 mx-0"
                        :class="routeWISel(wi.code, wi.number)"
                        @click.stop="$router.push({name:wi.code,params:{number:wi.number}})">
                    <v-layout row justify-end align-center>
                        <v-spacer/>
                            <span class="caption">
                                {{wi.code}}{{wi.number}}
                            </span>
                        <v-spacer/>
                        <v-btn small icon 
                        v-show="!mini" 
                        class="mx-0 my-0" 
                        @click.stop="dropWI(wi)" 
                        :class="routeWISel(wi.code, wi.number)">
                            <v-icon small>
                                close
                            </v-icon>
                        </v-btn>
                    </v-layout>
                </v-list-tile>
            </v-list>
        </v-navigation-drawer>
        <v-toolbar dense color="primary" dark app>
            <v-toolbar-side-icon @click.stop="mini = !mini"></v-toolbar-side-icon>
            <v-toolbar-title class="white--text hidden-sm-and-down">TEAM EDGE</v-toolbar-title>
            <v-spacer></v-spacer>
            <span class="white--text">{{profile.fullName}}</span>
            <v-avatar size="36px" class="ml-3">
                <v-img v-if="profile.avatar"
                :src="profile.avatar"
                alt="Avatar"/>
                <v-icon size="36px" dark v-else>account_circle</v-icon>
            </v-avatar>
            <side-menu/>
        </v-toolbar>
        
        <v-content>
            <v-container v-show="loading">
                <v-layout column justify-center align-center fill-height>
                    <v-progress-circular indeterminate color="primary"></v-progress-circular>
                </v-layout>
            </v-container>
            <router-view v-show="!loading"></router-view>
        </v-content>
        <create-work-item></create-work-item>
        <file-selector></file-selector>
    </div>
</template>

<script>
import sideMenu from '../side-menu'
import createWI from './create-wi.vue'
import fileSelector from './file-selector'
import {mapActions,mapGetters} from 'vuex'
export default {
    components:
    {
        'side-menu':sideMenu,
        'create-work-item':createWI,
        'file-selector':fileSelector
    },
    data:function(){
        return{
            drawer: true,
            items: [
            { title: 'dashboard', icon: 'backlog' },
            { title: 'About', icon: 'question_answer' }
            ],
            mini: true
        }
    },
    mounted(){
      var projId = this.$route.params.projId;
      this.fetchProj(projId);
    },
    methods:{
        ...mapActions({
           fetchProj: 'project/fetchProject',
           dropWI:'project/dropWI'
        }),
        goTo(name, id){
            this.$router.push({name:name});
            this.mini = true;
            this.selected = id;
        },
        routeSel(name){
            return this.$route.name === name?'selected':'notselected';
        },
        routeWISel(code,number){
            var flag = this.$route.name == code && this.$route.params.number == number;
            return flag?'selected':'notselected';
        }
    },
    computed:{
        ...mapGetters({
            loading:'project/loading',
            project:'project/project',
            profile:'auth/profile',
            workItems:'project/workItems'
        })
    }
}
</script>
<style scoped>
.selected{
    background: #fafafa;
    color: #424242;
    transition:none;
}
.notselected{
    background: #424242;
    color: #fafafa;
    transition:none;
}
.trans{
    background: transparent;
}
</style>

