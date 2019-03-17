<template>
    <v-layout row wrap>
        <v-flex md1>
        </v-flex>
        <v-flex xs12 md5>
            <v-card  class="mb-2">
                <v-toolbar color="transparent" flat>
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
                        <span>{{t.name}}</span>
                    </v-list-tile>
                </v-list>
                
            </v-card>
        </v-flex>
        <v-flex md1>
        </v-flex>
        <v-flex xs12 md4>
            <v-card>
                <v-toolbar color="transparent" flat>
                    <v-toolbar-title>Участники проекта</v-toolbar-title>
                    <v-spacer></v-spacer>
                    <v-btn icon>
                        <v-icon>refresh</v-icon>
                    </v-btn>
                </v-toolbar>
                <v-divider class="m-0"></v-divider>
                <v-list>
                    <v-list-tile v-for="(p,i) in project.partisipants" :key="i">
                        <v-list-tile-avatar color="primary">
                            <v-icon v-if="!p.avatar" size="32" color="white">account_circle</v-icon>
                            <img v-else :src="p.avatar"/>
                        </v-list-tile-avatar>
                        <v-list-tile-title>
                            <span>{{p.fullName}}</span>
                        </v-list-tile-title>
                    </v-list-tile>
                </v-list>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn small class="text-none">
                        Пригласить участника
                    </v-btn>
                </v-card-actions>
            </v-card>
        </v-flex>
        <v-flex md1>
        </v-flex>
    </v-layout>
</template>

<script>
import {mapGetters} from 'vuex'
export default {
    mounted(){
        this.fetchUserTasks(this.user.userId);
    },
    data:()=>({
        tasks:[]
    }),
    methods:{
        fetchUserTasks(userId){
            this.$http.get(`/api/workitems/project/${this.$route.params.projId}/user/${userId}`)
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
    }
}
</script>
