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
 

            <v-list class="pt-0">
                <v-divider class="m-0"></v-divider>

                <v-list-tile
                    @click="goTo('project')">
                    <v-list-tile-action>
                    <v-icon>dashboard</v-icon>
                    </v-list-tile-action>

                    <v-list-tile-content>
                    <v-list-tile-title>Доска</v-list-tile-title>
                    </v-list-tile-content>
                </v-list-tile>
            </v-list>
        </v-navigation-drawer>
        <v-toolbar dense color="primary" dark app>
            <v-toolbar-side-icon @click.stop="mini = !mini"></v-toolbar-side-icon>
            <v-toolbar-title v-if="ofSize({xs:false, sm:true})" class="white--text">TEAM EDGE</v-toolbar-title>
            <v-spacer></v-spacer>
            <span class="white--text">{{profile.fullName}}</span>
            <v-avatar size="36px" class="ml-3">
                <img v-if="profile.avatar"
                :src="profile.avatar"
                alt="Avatar">
                <v-icon size="36px" dark v-else>account_circle</v-icon>
            </v-avatar>
            <side-menu/>
        </v-toolbar>
        
        <v-content>
            <v-container fluid>
                <v-layout column justify-center align-center v-show="loading" fill-height>
                    <v-progress-circular indeterminate color="primary"></v-progress-circular>
                </v-layout>
                <router-view v-show="!loading"></router-view>
            </v-container>
        </v-content>
    </div>
</template>

<script>
import sideMenu from '../side-menu'
import onResize from '../../mixins/on-resize'
import {mapActions,mapGetters} from 'vuex'
export default {
    mixins:[onResize],
    components:
    {
        'side-menu':sideMenu
    },
    data:function(){
        return{
            drawer: true,
            items: [
            { title: 'Home', icon: 'dashboard' },
            { title: 'About', icon: 'question_answer' }
            ],
            mini: true,
            right: true
        }
    },
    mounted(){
      var projId = this.$route.params.projId;
      this.fetchProj(projId);
    },
    methods:{
        ...mapActions({
           fetchProj: 'project/fetchProject' 
        }),
        goTo(name){
            this.$router.push({name:name});
            this.mini = true;
        }
    },
    computed:{
        ...mapGetters({
            loading:'project/loading',
            project:'project/project',
            profile:'auth/profile'
        })
    }
}
</script>

