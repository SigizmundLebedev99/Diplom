<template>
    <v-container>
        <v-layout row wrap>
            <v-flex xs12 md6 class="px-3">
                <v-card class="mb-2">
                    <v-toolbar color="transparent" flat dense>
                        <v-toolbar-title>Ваши задачи</v-toolbar-title>
                        <v-spacer></v-spacer>
                        <v-btn icon @click="fetchUserTasks(user.userId)">
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
                            <item-chip :item="t"/>
                        </v-list-tile>    
                    </v-list>
                </v-card>
            </v-flex>
            <v-flex xs12 md6 class="px-3">
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
                                <span>{{p.name}}</span><span class="ml-1" v-if="p.role==1">(администратор)</span>
                            </v-list-tile-title>
                            <v-list-tile-action v-if="user.userId != p.id && role==1">
                                <user-menu :user="p"/>
                            </v-list-tile-action>
                        </v-list-tile>
                    </v-list>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <create-invite></create-invite>
                    </v-card-actions>
                </v-card>
            </v-flex>
        </v-layout>
    </v-container>
</template>

<script>
import {mapGetters} from 'vuex'
import createInvite from './create-invite'
import itemChip from '../item-chip'
import userMenu from './user-menu'
export default {
    components:{
        'create-invite': createInvite,
        'item-chip':itemChip,
        'user-menu':userMenu
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
            user:'auth/profile',
            role:'project/role'
        })
    }
}
</script>
