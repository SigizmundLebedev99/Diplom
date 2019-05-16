<template>
    <v-container>
        <v-layout row wrap>
            <v-flex md1>
            </v-flex>
            <v-flex xs12 md5>
                <v-card class="mb-2">
                    <v-toolbar color="transparent" flat dense>
                        <v-toolbar-title>Ваши задачи</v-toolbar-title>
                        <v-spacer></v-spacer>
                        <v-btn icon>
                            <v-icon>refresh</v-icon>
                        </v-btn>
                    </v-toolbar>
                    <v-divider class="m-0"></v-divider>
                    <v-container v-if="!tasks.length">
                        <v-layout justify-center>
                            <span>На данный момент у вас нет задач</span>
                        </v-layout>
                    </v-container>
                    <v-list v-else>
                        <v-list-tile v-for="(t,i) in tasks" :key="i">
                            <v-chip small label :color="workItems[t.code].color" class="white--text">
                                <span class="chip">{{t.code}}-{{t.number}}</span>
                            </v-chip>
                            <router-link class="ml-2" :to="{name:t.code, params:{number:t.number}}">{{t.name}}</router-link>
                        </v-list-tile>
                    </v-list>
                </v-card>
            </v-flex>
            <v-flex md1>
            </v-flex>
            <v-flex xs12 md4>
                <v-card>
                    <v-toolbar color="transparent" flat dense>
                        <v-toolbar-title>Участники проекта</v-toolbar-title>
                        <v-spacer></v-spacer>
                        <v-btn icon>
                            <v-icon>refresh</v-icon>
                        </v-btn>
                    </v-toolbar>
                    <v-divider class="m-0"></v-divider>
                    <v-list>
                        <v-list-tile v-for="(p,i) in project.partisipants" :key="i">
                            <v-list-tile-avatar color="primary" size="35">
                                <span v-if="!p.avatar" class="white--text text-xs-center mt-1" medium>
                                    {{`${p.name.split(' ').map(s=>s[0]).join('')}`}}
                                </span>
                                <v-img v-else :src="p.avatar"/>
                            </v-list-tile-avatar>
                            <v-list-tile-title>
                                <span>{{p.name}}</span>
                            </v-list-tile-title>
                        </v-list-tile>
                    </v-list>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <create-invite></create-invite>
                    </v-card-actions>
                </v-card>
            </v-flex>
            <v-flex md1>
            </v-flex>
        </v-layout>
    </v-container>
</template>

<script>
import {mapGetters} from 'vuex'
import createInvite from './create-invite'
import workItems from '../../data/work-items-object'
export default {
    components:{
        'create-invite': createInvite
    },
    mounted(){
        this.fetchUserTasks(this.user.userId);
    },
    data:()=>({
        tasks:[]
    }),
    methods:{
        loadTask(userId){
            return this.$http.get(`/api/workitems/project/${this.$route.params.projId}/user/${userId}`)
        },
        fetchUserTasks(userId){
            this.loadTask(userId)
            .then(
                r=>{this.tasks = r.data},
                r=>{console.log(r.response)}
            )
        }
    },
    computed:{
        ...mapGetters({
            project:'project/project',
            user:'auth/profile'
        }),
        workItems(){
            return workItems;
        }
    }
}
</script>
