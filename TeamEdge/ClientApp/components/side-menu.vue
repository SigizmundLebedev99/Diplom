<template>
    <v-menu offset-y>
        <template v-slot:activator="{ on }">
            <v-btn v-show="profile" icon v-on="on" flat>
                <v-icon>more_vert</v-icon>
            </v-btn>
        </template>
        <v-list>
            <v-list-tile @click="$router.push({name:'projects'})">    
                <v-list-tile-avatar>
                    <v-icon>
                        work
                    </v-icon>
                </v-list-tile-avatar>            
                <v-list-tile-title>Проекты</v-list-tile-title>
            </v-list-tile>
            <v-list-tile @click="signOut()">    
                <v-list-tile-avatar>
                    <v-icon>
                        mail
                    </v-icon>
                </v-list-tile-avatar>        
                <v-list-tile-title>Приглашения</v-list-tile-title>
            </v-list-tile>
            <v-list-tile @click="signOut()">    
                <v-list-tile-avatar>
                    <v-icon>
                        face
                    </v-icon>
                </v-list-tile-avatar>            
                <v-list-tile-title>Профиль</v-list-tile-title>
            </v-list-tile>
            <v-list-tile @click="signOut()">  
                <v-list-tile-avatar>
                    <v-icon>
                        cancel
                    </v-icon>
                </v-list-tile-avatar>               
                <v-list-tile-title>Выйти</v-list-tile-title>
            </v-list-tile>
        </v-list>
    </v-menu>
</template>

<script>
import { mapMutations, mapGetters } from 'vuex';
export default {
    methods:{
        ...mapMutations({
            signOut:'auth/signOut'
        })
    },
    computed:{
        ...mapGetters({
            profile:'auth/profile'
        })
    }
}
</script>

