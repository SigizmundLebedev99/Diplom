<template>
    <div>
        <v-layout row justify-space-between align-center :class="ofSize({xs:'mr-1 ml-1 mt-2 mb-2', sm:'mr-5 ml-5 mt-2 mb-2'})">
            <span class="title ml-3">
                Ваши проекты
            </span>
            <create-project></create-project>
        </v-layout>
        <v-divider></v-divider>
        <v-layout justify-center v-show="loading" class="contsraint mt-4">
            <v-progress-circular indeterminate color="primary"></v-progress-circular>
        </v-layout>
        <div v-show="!loading">
            <v-layout v-if="projects.length === 0" justify-space-around align-center>
                <span class="title text-xs-center contsraint mt-2">
                    На данный момент у вы не принимаете участия ни в каком проекте.
                </span>
            </v-layout>
            <v-container v-else>
                <v-layout wrap :class="ofSize({xs:'column align-center', sm:'row justify-center'})">
                    <v-card v-for="(p,i) in projects" :key="i" dark width="200px" class="mr-4 elevation-12">
                        <v-toolbar dark dense color="primary" flat>
                                <v-spacer></v-spacer>
                                <v-btn flat small class="text-none" @click="goToProj(p.id)">
                                    Перейти
                                    <v-icon dark rigth class="ml-2">arrow_forward</v-icon>
                                </v-btn>
                        </v-toolbar>
                        <v-card-title class="subheading">{{`${i + 1}) ${p.name}`}}</v-card-title>
                        <v-card-text>
                            <v-layout align-center column>
                                <v-avatar size="100" class="mb-3" color="white">
                                    <v-icon v-if="!p.logo" size="80" light color="primary">work</v-icon>
                                    <img v-else :src="p.logo"/>
                                </v-avatar>
                                <span>Вы {{partRoles[p.accessStatus]}}</span>
                                <v-layout row class="mt-3">
                                    <v-icon>people</v-icon>
                                    <span class="ml-2">Участников: {{p.usersCount}}</span>
                                </v-layout>
                                
                            </v-layout>
                        </v-card-text>
                    </v-card>
                </v-layout>
                <v-layout justify-center class="mt-5">
                    <v-btn color="primary" class="text-none" @click="fetchProjects()">Обновить</v-btn>
                </v-layout>
                
            </v-container>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapActions} from 'vuex'
import onResize from '../../mixins/on-resize'
import createProjects from './create-project'
import roles from '../../data/roles'
export default {
    mixins:[onResize],
    components:{
        'create-project':createProjects
    },
    methods:{
        ...mapActions({
            fetchProjects:'projects/fetchProjects'
        }),
        goToProj(projId){
            this.$router.push({name:'project', params:{projId:projId}})
        }
    },
    computed:{
        ...mapGetters({
                projects:'projects/projects',
                loading:'projects/loading'}),
        partRoles(){
            return roles;
        }
    }
}
</script>

<style scoped>
.contsraint{
    min-height: 450px;
}
</style>





