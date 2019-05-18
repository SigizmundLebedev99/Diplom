<template>
    <v-menu :close-on-content-click="false" v-model="model">
        <template v-slot:activator="{ on }">
            <v-btn icon v-on="on" flat small>
                <v-icon>
                    more_vert
                </v-icon>
            </v-btn>
        </template>
        <div>
            <v-list dense>
                <v-list-tile :disabled="user.role == 0" @click="toPart()">
                    <v-list-tile-title>
                    Сделать участником
                    </v-list-tile-title>
                </v-list-tile>
                <v-list-tile :disabled="user.role == 1" @click="toAdmin()">
                    <v-list-tile-title>
                    Сделать администратором
                    </v-list-tile-title>
                </v-list-tile>
                <v-list-tile @click="deletePart()">
                    <v-list-tile-title>
                    Удалить
                    </v-list-tile-title>
                </v-list-tile>
            </v-list>
        </div>
    </v-menu>
</template>

<script>
import {mapMutations, mapGetters} from 'vuex'
export default {
    data:()=>({
        model:false,
    }),
    props:[
        'user'
    ],
    methods:{
        ...mapMutations({
            openMessage:'message/open'
        }),
        toAdmin(){
            this.$http.put("/api/partisipant/status",{
                projectId:this.$route.params.projId,
                userId:this.user.id,
                newProjLevel:1
            }).then(r=>{
                this.user.role = 1;
            })
        },
        toPart(){
            this.$http.put("/api/partisipant/status",{
                projectId:this.$route.params.projId,
                userId:this.user.id,
                newProjLevel:0
            }).then(r=>{
                this.user.role = 0;
            })
        },
        deletePart(){
            this.openMessage({
                message:'Вы уверены, что хотите удалить этого участника?',
                callback:()=>{
                    this.$http.delete(`/api/partisipant/project/${this.$route.params.projId}/user/${this.user.id}`)
                    .then(r=>{
                        this.project.partisipants = this.project.partisipants.filter(e=>e.id != this.userId)
                        this.model = false;
                        })
                }
            });
        }
    },
    computed:{
        ...mapGetters({
            project:'project/project'
        })
    }
}
</script>

<style>

</style>
