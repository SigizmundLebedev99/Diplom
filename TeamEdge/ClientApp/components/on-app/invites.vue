<template>
    <div>
        <v-toolbar class="transparent" dense flat>
            <v-toolbar-title>
                Ваши инвайты
            </v-toolbar-title>
        </v-toolbar>
        <v-divider></v-divider>
        <v-container>
            <v-layout row wrap justify-center align-start>
                <span v-if="!invites.length">У вас нет ни одного инвайта</span>
                <v-card v-for="(invite,i) in invites" :key="i" dark width="220px" class="mr-4 elevation-12">
                    <v-layout column align-center dark class="primary pt-2 pb-2">
                        <v-chip class="mb-2">
                            <v-avatar>
                                <v-icon v-if="!invite.fromAvatar" light color="primary">account_circle</v-icon>
                                <img v-else :src="invite.fromAvatar"/>
                            </v-avatar>
                            {{invite.fromname}}
                        </v-chip>
                        кинул вам инвайт в проект
                    </v-layout>
                    <v-card-text class="pt-4 pb-2">
                        <v-layout align-center column>
                            <v-avatar size="100" class="mb-3" color="white">
                                <v-icon v-if="!invite.logo" size="80" light color="primary">work</v-icon>
                                <img v-else :src="invite.logo"/>
                            </v-avatar>
                            <span>{{invite.projectName}}</span>
                        </v-layout>
                    </v-card-text>
                    <v-footer dense color="primary" class="px-0 py-0" flat>
                        <v-spacer/>
                        <v-btn :loading='invite.loading' small flat class="primary text-none" @click="submit(invite)">
                            Принять
                        </v-btn>
                    </v-footer>
                </v-card>
            </v-layout>
        </v-container>
    </div>
</template>

<script>
import {mapGetters, mapActions} from 'vuex'
export default {
    methods:{
        ...mapActions({fetchProjects:'projects/fetchProjects'}),
        submit(invite){
            invite.loading = true;
            this.$http.post(`/api/partisipant/join/${invite.inviteId}`).then(
                r=>{
                    this.fetchProjects();
                    invite.loading = false;
                    this.$router.push({name:'projects'})                    
                },
                r=>{
                    console.log(r.response);
                    invite.loading = false;
                }
            )
        }
    },
    computed:{
        ...mapGetters({
            invites:'projects/invites'
        })
    }
}
</script>

<style>

</style>
